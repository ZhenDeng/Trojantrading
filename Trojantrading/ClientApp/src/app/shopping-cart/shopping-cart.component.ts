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
  displayedColumns: string[] = ['name', 'category', 'originalPrice', 'button'];

  constructor(
    private nav: NavbarService,
    private shareService: ShareService
  ) { }

  ngOnInit() {
    this.nav.hideTab();
    this.dataSource = this.shareService.product;
  }

}
