import { TestBed } from '@angular/core/testing';

import { IngredienteServiceService } from './ingrediente-service.service';

describe('IngredienteServiceService', () => {
  let service: IngredienteServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(IngredienteServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
