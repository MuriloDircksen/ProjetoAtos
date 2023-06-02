import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IEstoque } from 'src/app/models/estoque';


@Injectable({
  providedIn: 'root'
})
export class EstoqueServiceService {

  private url:string = "http://localhost:3000/estoques";

  constructor(private http: HttpClient) { }

  getIngredientes():Observable<IEstoque[]>{
    return this.http.get<IEstoque[]>(this.url);
  }

  salvarIngrediente(estoque: IEstoque): Observable<IEstoque>{
    return this.http.post<IEstoque>(this.url, estoque);
  }
  atualizarIngrediente(estoque: IEstoque): Observable<IEstoque> {
    return this.http.put<IEstoque>(`${this.url}/${estoque.id}`, estoque);
  }

  excluirIngrediente(id: number): Observable<IEstoque> {
    return this.http.delete<any>(`${this.url}/${id}`);
  }
}
