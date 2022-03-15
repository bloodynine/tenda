import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RepeatSettingsComponent } from './repeat-settings.component';

describe('RepeatSettingsComponent', () => {
  let component: RepeatSettingsComponent;
  let fixture: ComponentFixture<RepeatSettingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RepeatSettingsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RepeatSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
