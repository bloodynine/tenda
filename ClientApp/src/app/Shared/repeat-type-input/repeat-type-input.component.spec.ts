import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RepeatTypeInputComponent } from './repeat-type-input.component';

describe('RepeatTypeInputComponent', () => {
  let component: RepeatTypeInputComponent;
  let fixture: ComponentFixture<RepeatTypeInputComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RepeatTypeInputComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RepeatTypeInputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
