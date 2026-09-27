export interface Producto {
  id: number;
  codigo: string;
  nombre: string;
  descripcion?: string;
  precio: number;
  porcentajeIva: number;
  stock: number;
  activo: boolean;
}

export interface Cliente {
  id: number;
  tipoIdentificacion: number;
  identificacion: string;
  nombre: string;
  email?: string;
  telefono?: string;
  direccion?: string;
}

export interface DetalleFactura {
  productoId: number;
  productoNombre: string;
  cantidad: number;
  precioUnitario: number;
  porcentajeIva: number;
  subtotal: number;
}

export interface Factura {
  id: number;
  numero: string;
  fechaEmision: string;
  clienteNombre: string;
  clienteIdentificacion: string;
  detalles: DetalleFactura[];
  subtotal: number;
  totalIva: number;
  total: number;
  estado: number;
}

export interface CrearFactura {
  clienteId: number;
  detalles: { productoId: number; cantidad: number }[];
}
