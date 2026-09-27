import { Component, OnInit, inject, signal } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { Cliente } from '../../models/models';

@Component({
  selector: 'app-clientes',
  templateUrl: './clientes.html',
})
export class Clientes implements OnInit {
  private readonly api = inject(ApiService);

  readonly clientes = signal<Cliente[]>([]);
  readonly cargando = signal(true);

  ngOnInit() {
    this.api.getClientes().subscribe({
      next: (data) => {
        this.clientes.set(data);
        this.cargando.set(false);
      },
      error: () => this.cargando.set(false),
    });
  }
}
