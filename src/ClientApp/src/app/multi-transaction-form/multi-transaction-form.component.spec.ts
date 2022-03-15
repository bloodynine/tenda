import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MultiTransactionFormComponent } from './multi-transaction-form.component';

describe('MultiTransactionFormComponent', () => {
  let component: MultiTransactionFormComponent;
  let fixture: ComponentFixture<MultiTransactionFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MultiTransactionFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MultiTransactionFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
