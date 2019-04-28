import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavMenuComponent } from './nav-menu.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule, MatTableModule, MatInputModule, MatTabsModule, MatFormFieldModule, MatIconModule, MatCardModule, MatCheckboxModule, MatListModule, MatDialogModule } from '@angular/material';
import { productsRoutingModule } from '../products/products-routing.module.';
import { HttpClientModule } from '@angular/common/http';
import { RoutingModule } from '../app.routing';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterModule } from '@angular/router';
import { ProductsModule } from '../products/products.module';

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,
    FormsModule,
    RoutingModule,
    BrowserAnimationsModule,
    NgbModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatInputModule,
    MatTableModule,
    MatIconModule,
    MatCardModule,
    MatCheckboxModule,
    RouterModule,
    MatTabsModule,
    MatListModule,
    MatDialogModule
  ],
  declarations: [NavMenuComponent],
  exports:[
    NavMenuComponent
  ]
})
export class NavMenuModule { }
