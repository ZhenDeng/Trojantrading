import { RouterModule, Routes } from '@angular/router';
import { ModuleWithProviders } from '@angular/core';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './auth/auth.guard';

/* 
Don't use loadChildren: () => ProductDetailModule. 
Don't do that, prod build will fail, this is just a workaround.
Disable Angular Lazy Load Modules as it is not working in our application. Will inspect in the future if find solution to fix the bugs. 
*/
const appRoutes: Routes = [
  { path: 'home', component: HomeComponent, canActivate:[AuthGuard] },
  {
    path: 'login', component: LoginComponent
  },
  { path: '', redirectTo: '/login', pathMatch: 'full' }
];

export const RoutingModule: ModuleWithProviders = RouterModule.forRoot(appRoutes);
