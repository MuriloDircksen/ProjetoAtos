import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IReceitaIngrediente } from 'src/app/models/receita-ingrediente';
import { IReceitas } from 'src/app/models/receitas';

@Injectable({
  providedIn: 'root'
})
export class ReceitasServiceService {

  private url:string = "http://localhost:3000/receitas";
  private url2:string ="http://localhost:3000/receita_ingrediente"

  constructor(private http: HttpClient) { }

  getReceitas():Observable<IReceitas[]>{
    return this.http.get<IReceitas[]>(this.url);
  }
  getReceitaIngredientes():Observable<IReceitaIngrediente[]>{
    return this.http.get<IReceitaIngrediente[]>(this.url2);
  }
  getReceita(id: number): Observable<IReceitas> {
    return this.http.get<IReceitas>(`${this.url}/${id}`)
  }
  salvarReceita(receita: IReceitas): Observable<IReceitas>{
    return this.http.post<IReceitas>(this.url, receita);
  }
  salvarReceitaIngredientes(receitaIngrediente : IReceitaIngrediente) :Observable<IReceitaIngrediente>{
    return this.http.post<IReceitaIngrediente>(this.url2, receitaIngrediente);
  }
  atualizarReceita(receita: IReceitas): Observable<IReceitas> {
    return this.http.put<IReceitas>(`${this.url}/${receita.id}`, receita);
  }
  atualizarReceitaIngrediente(receitaIngrediente : IReceitaIngrediente) :Observable<IReceitaIngrediente>{
    return this.http.put<IReceitaIngrediente>(`${this.url2}/${receitaIngrediente.id}`, receitaIngrediente);
  }
  excluirReceita(id: number): Observable<IReceitas> {
    return this.http.delete<any>(`${this.url}/${id}`);
  }
  excluirReceitaIngrediente(id: number): Observable<IReceitas> {
    return this.http.delete<any>(`${this.url2}/${id}`);
  }
}
