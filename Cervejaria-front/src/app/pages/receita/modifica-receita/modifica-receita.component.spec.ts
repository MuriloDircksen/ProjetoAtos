import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModificaReceitaComponent } from './modifica-receita.component';

describe('ModificaReceitaComponent', () => {
  let component: ModificaReceitaComponent;
  let fixture: ComponentFixture<ModificaReceitaComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ModificaReceitaComponent]
    });
    fixture = TestBed.createComponent(ModificaReceitaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
