import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ByTagReportComponent } from './by-tag-report.component';

describe('ByTagReportComponent', () => {
  let component: ByTagReportComponent;
  let fixture: ComponentFixture<ByTagReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ByTagReportComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ByTagReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
