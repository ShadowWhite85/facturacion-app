import { Component, OnInit, inject, signal } from '@angular/core';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { ApiService } from '../../services/api.service';
import { Factura } from '../../models/models';

@Component({
  selector: 'app-facturas',
  imports: [CurrencyPipe, DatePipe],
  templateUrl: './facturas.html',
})
export class Facturas implements OnInit {
  private readonly api = inject(ApiService);

  readonly facturas = signal<Factura[]>([]);
  readonly cargando = signal(true);
  readonly expandida = signal<number | null>(null);

  ngOnInit() {
    this.cargar();
  }

  cargar() {
    this.api.getFacturas().subscribe({
      next: (data) => {
        this.facturas.set(data);
        this.cargando.set(false);
      },
      error: () => this.cargando.set(false),
    });
  }

  anular(id: number) {
    this.api.anularFactura(id).subscribe(() => this.cargar());
  }
}
