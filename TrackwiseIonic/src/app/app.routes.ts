import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'tabs',
    loadChildren: () => import('./tabs/tabs.routes').then((m) => m.routes),
  },
  {
    path: '',
    loadComponent: () => import('./pages/login/login.page').then( m => m.LoginPage)
  },
  {
    path: 'login',
    loadComponent: () => import('./pages/login/login.page').then( m => m.LoginPage)
  },
  {
    path: 'forgotpassword',
    loadComponent: () => import('./pages/forgotpassword/forgotpassword.page').then( m => m.ForgotpasswordPage)
  },
];
