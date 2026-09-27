import { Component, OnInit, inject, signal } from '@angular/core';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { ApiService } from '../../services/api.service';
import { AuthService } from '../../services/auth.service';
import { Factura } from '../../models/models';

@Component({
  selector: 'app-facturas',
  imports: [CurrencyPipe, DatePipe],
  templateUrl: './facturas.html',
})
export class Facturas implements OnInit {
  private readonly api = inject(ApiService);
  readonly auth = inject(AuthService);

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

  descargar(id: number, numero: string, formato: 'pdf' | 'xml') {
    const peticion = formato === 'pdf' ? this.api.descargarPdf(id) : this.api.descargarXml(id);
    peticion.subscribe((blob) => {
      const url = URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = `factura-${numero}.${formato}`;
      a.click();
      URL.revokeObjectURL(url);
    });
  }
}
