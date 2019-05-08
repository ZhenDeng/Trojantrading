import { FileService } from './services/file.service';
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
import { MatTabsModule, MatTableModule, MatDialogModule } from '@angular/material';
import { AccountDetailsComponent } from './account-details/account-details.component';
import { AdminService } from './services/admin.service';
import { AboutUsComponent } from './about-us/about-us.component';
import { ContactUsComponent } from './contact-us/contact-us.component';
import { PromoSummaryComponent } from './promo-summary/promo-summary.component';
import { TermsComponent } from './terms/terms.component';
import { NavbarService } from './services/navbar.service';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { AuthRecovery } from './auth/auth.recovery';
import { ShareService } from './services/share.service';
import { PasswordRecoveryComponent } from './password-recovery/password-recovery.component';
import { RouterModule } from '@angular/router';
import { ProductsModule } from './products/products.module';
import { ShoppingCartComponent } from './shopping-cart/shopping-cart.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { MatListModule } from '@angular/material/list';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { OrdersComponent } from './orders/orders.component';
import { TermsAndConditionsComponent } from './popup-collection/terms-and-conditions/terms-and-conditions.component';
import { ShoppingCartService } from './services/shopping-cart.service';
import { AdministrationComponent } from './administration/administration.component';
import { NavMenuModule } from './nav-menu/nav-menu.module';
import { OrderService } from './services/order.service';
import { EditProductComponent } from './popup-collection/edit-product/edit-product.component';
import { MatSelectModule } from '@angular/material/select';
import { EditUserComponent } from './popup-collection/edit-user/edit-user.component';
import { EditAddressComponent } from './popup-collection/edit-address/edit-address.component';
import { SlickCarouselModule } from 'ngx-slick-carousel';
import { NgbdCarouselBasicModule } from './ngbd-carousel-basic/ngbd-carousel-basic.module';
import { EditOrderComponent } from './popup-collection/edit-order/edit-order.component';
import { EditHeaderInfomationComponent } from './popup-collection/edit-header-infomation/edit-header-infomation.component';
import { HeadInformationService } from './services/head-information.service';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    LoginComponent,
    AccountDetailsComponent,
    AboutUsComponent,
    ContactUsComponent,
    PromoSummaryComponent,
    TermsComponent,
    PasswordRecoveryComponent,
    ShoppingCartComponent,
    OrdersComponent,
    TermsAndConditionsComponent,
    AdministrationComponent,
    EditProductComponent,
    EditUserComponent,
    EditAddressComponent,
    EditOrderComponent,
    EditHeaderInfomationComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RoutingModule,
    BrowserAnimationsModule,
    NgbModule.forRoot(),
    MatFormFieldModule,
    ReactiveFormsModule,
    MatInputModule,
    MatTableModule,
    MatIconModule,
    MatCardModule,
    MatCheckboxModule,
    RouterModule,
    MatTabsModule,
    ProductsModule,
    MatListModule,
    MatDialogModule,
    NavMenuModule,
    MatSelectModule,
    SlickCarouselModule,
    NgbdCarouselBasicModule
  ],
  providers: [
    UserService,
    AdminService,
    NavbarService,
    AuthGuard,
    AuthRecovery,
    ShareService,
    ShoppingCartService,
    OrderService,
    FileService,
    HeadInformationService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }],
  entryComponents: [
    TermsAndConditionsComponent,
    EditProductComponent,
    EditUserComponent,
    EditAddressComponent,
    EditHeaderInfomationComponent,
    EditOrderComponent
  ],
  bootstrap: [AppComponent]
})

export class AppModule { }
