import { Component } from '@angular/core';

@Component({
  selector: 'app-ngbd-carousel-basic',
  templateUrl: './ngbd-carousel-basic.component.html'
})
export class NgbdCarouselBasicComponent {

  images: string[] = ["/img/slide1.png", "/img/slide2.png", "/img/slide3.png"];
  slideConfig = {
    "autoplay": true,
    "arrows": true,
    "dots": true,
    "adaptiveHeight": true,
    "pauseOnHover": true
  };

}
