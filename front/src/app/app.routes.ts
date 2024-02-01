import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '', loadComponent: () => import('./components/container/container.component').then(mod => mod.ContainerComponent), children: [
      { path: 'home', loadComponent: () => import('./components/users-management/users-management.component').then(mod => mod.UsersManagementComponent) },
      { path: '', redirectTo: 'home', pathMatch: 'full' }
    ]
  }
]
