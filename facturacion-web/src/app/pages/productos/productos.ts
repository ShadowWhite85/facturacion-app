import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DecimalPipe } from '@angular/common';
import { ApiService } from '../../services/api.service';
import { Producto } from '../../models/models';

@Component({
  selector: 'app-productos',
  imports: [FormsModule, DecimalPipe],
  templateUrl: './productos.html',
})
export class Productos implements OnInit {
  private readonly api = inject(ApiService);

  readonly productos = signal<Producto[]>([]);
  readonly busqueda = signal('');
  readonly cargando = signal(false);
  readonly error = signal('');

  nuevo = { codigo: '', nombre: '', descripcion: '', precio: 0, porcentajeIva: 15, stock: 0 };

  ngOnInit() {
    this.cargar();
  }

  cargar() {
    this.cargando.set(true);
    this.api.getProductos(this.busqueda() || undefined).subscribe({
      next: (data) => {
        this.productos.set(data);
        this.cargando.set(false);
      },
      error: () => {
        this.error.set('No se pudo conectar con la API. ¿Está corriendo en http://localhost:5059?');
        this.cargando.set(false);
      },
    });
  }

  crear() {
    this.api.crearProducto(this.nuevo).subscribe({
      next: () => {
        this.nuevo = { codigo: '', nombre: '', descripcion: '', precio: 0, porcentajeIva: 15, stock: 0 };
        this.cargar();
      },
      error: (e) => this.error.set(e.error?.mensaje ?? 'Error al crear el producto'),
    });
  }
}
