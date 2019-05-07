import { Component, OnInit } from '@angular/core';
import { HeadInformationService } from '../services/head-information.service';
import { HeadInformation } from '../models/header-info';

@Component({
  selector: 'app-ngbd-carousel-basic',
  templateUrl: './ngbd-carousel-basic.component.html'
})
export class NgbdCarouselBasicComponent implements OnInit {

  slideConfig = {
    "autoplay": true,
    "arrows": true,
    "dots": true,
    "adaptiveHeight": true,
    "pauseOnHover": true
  };

  headerDataSource: HeadInformation[] = [];

  constructor(
    private headInformationService: HeadInformationService
  ) { }

  ngOnInit(): void {
    this.headInformationService.currentMessage.subscribe((result: Number) => {
      this.headInformationService.GetHeadInformation().subscribe((res: HeadInformation[]) => {
        if (res) {
          this.headerDataSource = res;
        }
      },
        (error: any) => {
          console.info(error);
        });
    },
      (error: any) => {
        console.info(error);
      });
  }

}
