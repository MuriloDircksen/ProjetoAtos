import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModificaIngredienteComponent } from './modifica-ingrediente.component';

describe('ModificaIngredienteComponent', () => {
  let component: ModificaIngredienteComponent;
  let fixture: ComponentFixture<ModificaIngredienteComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ModificaIngredienteComponent]
    });
    fixture = TestBed.createComponent(ModificaIngredienteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
