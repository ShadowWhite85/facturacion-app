import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '../../environments/environment';
import { Cliente, CrearFactura, Factura, Producto } from '../models/models';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private readonly http = inject(HttpClient);
  private readonly base = environment.apiUrl;

  getProductos(busqueda?: string) {
    return this.http.get<Producto[]>(`${this.base}/productos`, {
      params: busqueda ? { busqueda } : {},
    });
  }

  crearProducto(producto: Omit<Producto, 'id' | 'activo'>) {
    return this.http.post<Producto>(`${this.base}/productos`, producto);
  }

  getClientes() {
    return this.http.get<Cliente[]>(`${this.base}/clientes`);
  }

  getFacturas() {
    return this.http.get<Factura[]>(`${this.base}/facturas`);
  }

  crearFactura(factura: CrearFactura) {
    return this.http.post<Factura>(`${this.base}/facturas`, factura);
  }

  anularFactura(id: number) {
    return this.http.post(`${this.base}/facturas/${id}/anular`, {});
  }

  descargarPdf(id: number) {
    return this.http.get(`${this.base}/facturas/${id}/pdf`, { responseType: 'blob' });
  }

  descargarXml(id: number) {
    return this.http.get(`${this.base}/facturas/${id}/xml`, { responseType: 'blob' });
  }
}
