import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RepeatFormComponent } from './repeat-form.component';

describe('RepeatFormComponent', () => {
  let component: RepeatFormComponent;
  let fixture: ComponentFixture<RepeatFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RepeatFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RepeatFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
