import { Injectable } from '@angular/core';

@Injectable()
export class NavbarService {
  visible: boolean;
  tabVisible: boolean;

  constructor() { 
    this.visible = false;
    this.tabVisible = false; 
  }

  hide() { this.visible = false; }

  show() { this.visible = true; }

  toggle() { this.visible = !this.visible; }

  hideTab() { this.tabVisible = false; }

  showTab() { this.tabVisible = true; }

  toggleTab() { this.tabVisible = !this.tabVisible; }

}