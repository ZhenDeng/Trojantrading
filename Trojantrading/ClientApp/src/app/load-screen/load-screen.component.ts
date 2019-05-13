import { Subscription } from 'rxjs';
import { Component, OnInit, OnDestroy, Output, EventEmitter, Input } from '@angular/core';
import { debounceTime } from 'rxjs/operators';
import { LoadScreenService } from '../services/load-screen.service';
 
 
@Component({
  selector: 'loading-screen',
  templateUrl: './loading-screen.component.html',
  styleUrls: ['./loading-screen.component.css']
})
export class LoadingScreenComponent implements OnInit, OnDestroy {
  @Input('backDropWidth') backDropWidth: string;
  @Input('showBlurTable') showBlurTable: boolean;
  @Input('isScrolled') isScrolled: boolean;
  @Input('scrollAfterLoading') scrollAfterLoading: boolean;
  @Output() currentLoadingStatus = new EventEmitter<boolean>();
 
 
  loading: boolean = true;
  loadingSubscription: Subscription;
  currentWidth: string;
  showTable: boolean = false;
 
  constructor(
    private loadingScreenService: LoadScreenService
  ) { }
  
  ngOnInit() {
 
    this.currentWidth = this.backDropWidth ? this.backDropWidth : '984';
    this.showTable = this.showBlurTable ? this.showBlurTable : false;
 
    this.loadingSubscription = this.loadingScreenService.loadingStatus.pipe(
      debounceTime(200)
    ).subscribe((value) => {
 
      this.loading = value;
      
      this.currentLoadingStatus.emit(value);
 
      //TODO: customize for subscriptions pages for now, need more input if other pages wanna use it
      // if (this.scrollAfterLoading) {
      //    $('#levelOne').animate({ scrollLeft: 250 }, 400);
      // }
    });
  }
 
  ngOnDestroy() {
    this.loadingSubscription.unsubscribe();
  }
 
}