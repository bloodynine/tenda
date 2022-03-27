import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectionReportComponent } from './projection-report.component';

describe('ProjectionReportComponent', () => {
  let component: ProjectionReportComponent;
  let fixture: ComponentFixture<ProjectionReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectionReportComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectionReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
