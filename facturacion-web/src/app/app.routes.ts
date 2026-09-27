import { Routes } from '@angular/router';
import { Productos } from './pages/productos/productos';
import { Clientes } from './pages/clientes/clientes';
import { Facturas } from './pages/facturas/facturas';
import { NuevaFactura } from './pages/nueva-factura/nueva-factura';
import { Login } from './pages/login/login';
import { authGuard } from './services/auth.guard';

export const routes: Routes = [
  { path: 'login', component: Login, title: 'Iniciar sesión' },
  { path: '', redirectTo: 'facturas', pathMatch: 'full' },
  { path: 'facturas', component: Facturas, title: 'Facturas', canActivate: [authGuard] },
  { path: 'facturas/nueva', component: NuevaFactura, title: 'Nueva factura', canActivate: [authGuard] },
  { path: 'productos', component: Productos, title: 'Productos', canActivate: [authGuard] },
  { path: 'clientes', component: Clientes, title: 'Clientes', canActivate: [authGuard] },
  { path: '**', redirectTo: 'facturas' },
];
