import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PromoSummaryComponent } from './promo-summary.component';

describe('PromoSummaryComponent', () => {
  let component: PromoSummaryComponent;
  let fixture: ComponentFixture<PromoSummaryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PromoSummaryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PromoSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
