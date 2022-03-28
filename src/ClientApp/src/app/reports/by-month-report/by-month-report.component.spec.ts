import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ByMonthReportComponent } from './by-month-report.component';

describe('ByMonthReportComponent', () => {
  let component: ByMonthReportComponent;
  let fixture: ComponentFixture<ByMonthReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ByMonthReportComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ByMonthReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
