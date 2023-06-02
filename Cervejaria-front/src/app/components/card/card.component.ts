import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.scss']
})
export class CardComponent {
  @Input()totalCusto: number = 0;
  @Input()cervejasProduzidas: number = 0;
  @Input()volumeProduzido: number = 0;
}
