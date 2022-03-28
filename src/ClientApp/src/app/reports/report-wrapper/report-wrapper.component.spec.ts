import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportWrapperComponent } from './report-wrapper.component';

describe('ReportWrapperComponent', () => {
  let component: ReportWrapperComponent;
  let fixture: ComponentFixture<ReportWrapperComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReportWrapperComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportWrapperComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
