import { NgModule } from '@angular/core';
import { NgbdCarouselBasicComponent } from './ngbd-carousel-basic.component';
import { BrowserModule } from '@angular/platform-browser';
import { SlickCarouselModule } from 'ngx-slick-carousel';

@NgModule({
  imports: [BrowserModule, SlickCarouselModule],
  declarations: [NgbdCarouselBasicComponent],
  exports: [NgbdCarouselBasicComponent]
})
export class NgbdCarouselBasicModule { }
