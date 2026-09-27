import { Injectable, computed, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { environment } from '../../environments/environment';

export interface SesionUsuario {
  token: string;
  nombre: string;
  email: string;
  rol: string;
  expira: string;
}

const CLAVE = 'facturacion.sesion';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);

  readonly usuario = signal<SesionUsuario | null>(this.leerSesion());

  readonly autenticado = computed(() => {
    const u = this.usuario();
    return !!u && new Date(u.expira) > new Date();
  });

  readonly esAdmin = computed(() => this.usuario()?.rol === 'Admin');

  login(email: string, password: string) {
    return this.http
      .post<SesionUsuario>(`${environment.apiUrl}/auth/login`, { email, password })
      .pipe(
        tap((sesion) => {
          localStorage.setItem(CLAVE, JSON.stringify(sesion));
          this.usuario.set(sesion);
          this.router.navigate(['/facturas']);
        })
      );
  }

  logout() {
    localStorage.removeItem(CLAVE);
    this.usuario.set(null);
    this.router.navigate(['/login']);
  }

  token(): string | null {
    return this.autenticado() ? this.usuario()!.token : null;
  }

  private leerSesion(): SesionUsuario | null {
    try {
      const raw = localStorage.getItem(CLAVE);
      return raw ? (JSON.parse(raw) as SesionUsuario) : null;
    } catch {
      return null;
    }
  }
}
