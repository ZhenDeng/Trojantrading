import { RouterModule, Routes } from '@angular/router';
import { ModuleWithProviders } from '@angular/core';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './auth/auth.guard';
import { AccountDetailsComponent } from './account-details/account-details.component';
import { AboutUsComponent } from './about-us/about-us.component';
import { ContactUsComponent } from './contact-us/contact-us.component';
import { PromoSummaryComponent } from './promo-summary/promo-summary.component';
import { TermsComponent } from './terms/terms.component';
import { PasswordRecoveryComponent } from './password-recovery/password-recovery.component';
import { AuthRecovery } from './auth/auth.recovery';
import { ShoppingCartComponent } from './shopping-cart/shopping-cart.component';
import { OrdersComponent } from './orders/orders.component';
import { AdministrationComponent } from './administration/administration.component';

const appRoutes: Routes = [
  { path: 'home', component: HomeComponent, canActivate: [AuthGuard] },
  { path: 'account', component: AccountDetailsComponent, canActivate: [AuthGuard] },
  { path: 'recover/:token', component: PasswordRecoveryComponent, canActivate: [AuthRecovery] },
  {
    path: 'login', component: LoginComponent
  },

  { path: 'about-us', component: AboutUsComponent, canActivate: [AuthGuard] },
  { path: 'contact-us', component: ContactUsComponent, canActivate: [AuthGuard] },
  { path: 'promo', component: PromoSummaryComponent, canActivate: [AuthGuard] },
  { path: 'terms', component: TermsComponent, canActivate: [AuthGuard] },
  { path: 'cart', component: ShoppingCartComponent, canActivate: [AuthGuard] },
  { path: 'orders', component: OrdersComponent, canActivate: [AuthGuard]},
  { path: 'admin', component: AdministrationComponent, canActivate: [AuthGuard]},
  {
    path: '',
    redirectTo: '/home',
    pathMatch: 'full'
  }
];

export const RoutingModule: ModuleWithProviders = RouterModule.forRoot(appRoutes);
