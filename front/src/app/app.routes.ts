import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '', loadComponent: () => import('./components/container/container.component').then(mod => mod.ContainerComponent), children: [
      //{ path: 'home', loadComponent: () => import('./components/denunciante-home/denunciante-home.component').then(mod => mod.DenuncianteHomeComponent) },
      //{ path: '', redirectTo: 'home', pathMatch: 'full' }
    ]
  }
];
