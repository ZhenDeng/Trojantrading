import { Component } from '@angular/core';
import { NgbCarouselConfig } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-ngbd-carousel-basic',
  templateUrl: './ngbd-carousel-basic.component.html',
  styleUrls: ['../../../node_modules/bootstrap/dist/css/bootstrap.css'],
  providers: [NgbCarouselConfig]
})
export class NgbdCarouselBasicComponent {

  images: string[] = ["/img/slide1.jpg", "/img/slide2.jpg", "/img/slide3.jpg"];

  constructor(
    private config: NgbCarouselConfig
  ) { 
    this.config.interval = 10000;
    this.config.wrap = false;
    this.config.keyboard = false;
  }

}
