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

/* 
Don't use loadChildren: () => ProductDetailModule. 
Don't do that, prod build will fail, this is just a workaround.
Disable Angular Lazy Load Modules as it is not working in our application. Will inspect in the future if find solution to fix the bugs. 
*/
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

  {
    path: '',
    redirectTo: '/home',
    pathMatch: 'full'
  }
];

export const RoutingModule: ModuleWithProviders = RouterModule.forRoot(appRoutes);
