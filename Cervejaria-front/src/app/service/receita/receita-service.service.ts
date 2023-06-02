import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IReceitas } from 'src/app/models/receitas';

@Injectable({
  providedIn: 'root'
})
export class ReceitasServiceService {

  private url:string = "http://localhost:3000/receitas";

  constructor(private http: HttpClient) { }

  getReceitas():Observable<IReceitas[]>{
    return this.http.get<IReceitas[]>(this.url);
  }

  salvarReceita(receita: IReceitas): Observable<IReceitas>{
    return this.http.post<IReceitas>(this.url, receita);
  }
  atualizarReceita(receita: IReceitas): Observable<IReceitas> {
    return this.http.put<IReceitas>(`${this.url}/${receita.id}`, receita);
  }

  excluirReceita(id: number): Observable<IReceitas> {
    return this.http.delete<any>(`${this.url}/${id}`);
  }
}
