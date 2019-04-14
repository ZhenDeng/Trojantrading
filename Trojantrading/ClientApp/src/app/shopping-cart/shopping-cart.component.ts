import { Component, OnInit } from '@angular/core';
import { NavbarService } from '../services/navbar.service';
import { Product } from '../models/Product';
import { ShareService } from '../services/share.service';

@Component({
  selector: 'app-shopping-cart',
  templateUrl: './shopping-cart.component.html',
  styleUrls: ['./shopping-cart.component.css']
})
export class ShoppingCartComponent implements OnInit {

  dataSource: Product[];
  totalPrice: number;
  displayedColumns: string[] = ['name', 'category', 'originalPrice', 'qty', 'subTotal', 'remove'];

  constructor(
    private nav: NavbarService,
    private shareService: ShareService
  ) { }

  ngOnInit() {
    this.nav.hideTab();
    this.totalPrice = 0;
    this.dataSource = this.shareService.product;
    this.dataSource.forEach(product => {
      this.totalPrice += product.quantity * product.originalPrice;
    });
  }

  _keyPress(event: any) {
    const pattern = /[0-9\+\-\ ]/;
    let inputChar = String.fromCharCode(event.charCode);

    if (!pattern.test(inputChar)) {
      // invalid character, prevent input
      event.preventDefault();
    }
  }


}
