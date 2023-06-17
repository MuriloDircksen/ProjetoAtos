import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IProducao } from 'src/app/models/producao';

@Injectable({
  providedIn: 'root'
})
export class ProducaoServiceService {

  private url:string = "http://localhost:3000/producao";

  constructor(private http: HttpClient) { }

  getProducoes():Observable<IProducao[]>{
    return this.http.get<IProducao[]>(this.url);
  }
  getProducao(id:number):Observable<IProducao[]>{
    return this.http.get<IProducao[]>(`${this.url}/${id}`);
  }

  salvarProducao(producao: IProducao): Observable<IProducao>{
    return this.http.post<IProducao>(this.url, producao);
  }
  atualizarProducao(producao: IProducao): Observable<IProducao> {
    return this.http.put<IProducao>(`${this.url}/${producao.id}`, producao);
  }
  excluirProducao(id: number): Observable<IProducao> {
    return this.http.delete<any>(`${this.url}/${id}`);
  }
}
