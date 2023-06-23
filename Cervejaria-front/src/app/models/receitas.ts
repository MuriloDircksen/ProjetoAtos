import { IProducao } from "./producao";

export interface IReceitas{
  id: number,
  nomeReceita: string,
  responsavel: string,
  estilo: string,
  ultimaAtualizacao: Date,
  orcamento: number,
  volumeReceita: number
}
