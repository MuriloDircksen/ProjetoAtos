import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProduzirComponent } from './produzir.component';

describe('ProduzirComponent', () => {
  let component: ProduzirComponent;
  let fixture: ComponentFixture<ProduzirComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ProduzirComponent]
    });
    fixture = TestBed.createComponent(ProduzirComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
