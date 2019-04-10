import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { RoutingModule } from './app.routing';
import { UserService } from './services/user.service';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './auth/auth.guard';
import { AuthInterceptor } from './auth/auth.interceptor';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { AccountDetailsComponent } from './account-details/account-details.component';
import { AdminService } from './services/admin.service';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { AboutUsComponent } from './about-us/about-us.component';
import { ContactUsComponent } from './contact-us/contact-us.component';
import { PromoSummaryComponent } from './promo-summary/promo-summary.component';
import { TermsComponent } from './terms/terms.component';
import { NavbarService } from './services/navbar.service';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { AuthRecovery } from './auth/auth.recovery';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    LoginComponent,
    AccountDetailsComponent,
    NavMenuComponent,
    AboutUsComponent,
    ContactUsComponent,
    PromoSummaryComponent,
    TermsComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RoutingModule,
    BrowserAnimationsModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatInputModule,
    MatIconModule,
    MatTableModule,
    MatCardModule
  ],
  providers: [
    UserService,
    AdminService,
    NavbarService,
    AuthGuard,
    AuthRecovery,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }],
  bootstrap: [AppComponent]
})

export class AppModule { }
