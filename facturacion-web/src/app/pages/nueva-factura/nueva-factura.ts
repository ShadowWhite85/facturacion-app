import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CurrencyPipe } from '@angular/common';
import { Router } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { Cliente, Producto } from '../../models/models';

interface LineaFactura {
  productoId: number;
  cantidad: number;
}

@Component({
  selector: 'app-nueva-factura',
  imports: [FormsModule, CurrencyPipe],
  templateUrl: './nueva-factura.html',
})
export class NuevaFactura implements OnInit {
  private readonly api = inject(ApiService);
  private readonly router = inject(Router);

  readonly clientes = signal<Cliente[]>([]);
  readonly productos = signal<Producto[]>([]);
  readonly clienteId = signal<number>(0);
  readonly lineas = signal<LineaFactura[]>([{ productoId: 0, cantidad: 1 }]);
  readonly error = signal('');

  readonly totalEstimado = computed(() =>
    this.lineas().reduce((acc, l) => {
      const p = this.productos().find((x) => x.id === l.productoId);
      if (!p) return acc;
      return acc + p.precio * l.cantidad * (1 + p.porcentajeIva / 100);
    }, 0)
  );

  ngOnInit() {
    this.api.getClientes().subscribe((c) => this.clientes.set(c));
    this.api.getProductos().subscribe((p) => this.productos.set(p));
  }

  agregarLinea() {
    this.lineas.update((l) => [...l, { productoId: 0, cantidad: 1 }]);
  }

  actualizarLinea(index: number, cambios: Partial<LineaFactura>) {
    this.lineas.update((l) => l.map((linea, i) => (i === index ? { ...linea, ...cambios } : linea)));
  }

  emitir() {
    this.error.set('');
    this.api
      .crearFactura({
        clienteId: this.clienteId(),
        detalles: this.lineas().filter((l) => l.productoId > 0 && l.cantidad > 0),
      })
      .subscribe({
        next: () => this.router.navigate(['/facturas']),
        error: (e) => this.error.set(e.error?.mensaje ?? 'Error al emitir la factura'),
      });
  }
}
