import { Routes } from '@angular/router';
import { Productos } from './pages/productos/productos';
import { Clientes } from './pages/clientes/clientes';
import { Facturas } from './pages/facturas/facturas';
import { NuevaFactura } from './pages/nueva-factura/nueva-factura';

export const routes: Routes = [
  { path: '', redirectTo: 'facturas', pathMatch: 'full' },
  { path: 'facturas', component: Facturas, title: 'Facturas' },
  { path: 'facturas/nueva', component: NuevaFactura, title: 'Nueva factura' },
  { path: 'productos', component: Productos, title: 'Productos' },
  { path: 'clientes', component: Clientes, title: 'Clientes' },
];
