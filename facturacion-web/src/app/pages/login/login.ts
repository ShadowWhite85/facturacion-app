import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  imports: [FormsModule],
  templateUrl: './login.html',
})
export class Login {
  private readonly auth = inject(AuthService);

  email = '';
  password = '';
  readonly error = signal('');
  readonly cargando = signal(false);

  entrar() {
    this.error.set('');
    this.cargando.set(true);
    this.auth.login(this.email, this.password).subscribe({
      error: () => {
        this.error.set('Email o contraseña incorrectos');
        this.cargando.set(false);
      },
    });
  }
}
