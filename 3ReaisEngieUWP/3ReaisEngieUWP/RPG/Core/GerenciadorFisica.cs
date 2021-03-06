﻿using _3ReaisEngine.Components;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace _3ReaisEngine.Core
{
    public class GerenciadorFisica
    {
       
        float GetAngle(Vector2 a,Vector2 b, Vector2 eixo)
        {
            float d = Engine.Distance(a, b);
            float h = 0;
            float sin;
            if(eixo.x != 0)
            {
                h = Math.Abs(a.y - b.y);
            }
            if (eixo.y != 0)
            {
                h = Math.Abs(a.x - b.x);
            }

            sin = h / d;

            return (float)(Math.Asin(sin)*(180.0f/Math.PI));
        }
        
        public void UpdateColisions(Colisao[] array)
        {
            int lenght = array.Length;
            Vector2 distance;
            Vector2 safeDist = Vector2.Zero;
            float angleCol;
            Vector2 dir;
            Vector2 velocity;
            Body b;

            for (int i = 0; i < lenght; i++)
            {
                if (array[i].tipo == TipoColisao.Estatica) continue;
                velocity = Vector2.Zero;
                array[i].momentoDeColisao.w = 0;
                array[i].momentoDeColisao.x = 0;
                array[i].momentoDeColisao.y = 0;
                array[i].momentoDeColisao.z = 0;

                
                if((b = array[i].entidade.GetComponente<Body>()) != null) velocity = b.velocity;

                for (int j = 0; j < lenght; j++)
                {
                    if (array[i].entidade.ID == array[j].entidade.ID || array[i].ignoreTypes.Contains(array[j].entidade.GetType())) continue;
                                    
                    distance = Engine.DistanceVec(array[i].Position, array[j].Position);
                    safeDist.x = (array[i].tamanho.x + array[j].tamanho.x) / 2.0f;
                    safeDist.y = (array[i].tamanho.y + array[j].tamanho.y) / 2.0f;

                    if ((distance.x + velocity.x) > safeDist.x || (distance.y + velocity.y) > safeDist.y) continue;

                    angleCol = GetAngle(array[i].Position, array[j].Position, new Vector2(1, 0));
                    dir = (angleCol < GetAngle(array[i].Position, (array[i].Position + array[i].tamanho / 2.0f), Vector2.right) ? Vector2.right : Vector2.up);
                 
                    if(!array[j].IsTrigger && !array[i].IsTrigger)
                    {
                        if (array[i].Position.x + velocity.x > array[j].Position.x && dir == Vector2.right)
                        {
                            array[i].momentoDeColisao.z = 1;
                            if (velocity.x < 0) velocity.x = 0;
                        }
                        if (array[i].Position.x + velocity.x < array[j].Position.x && dir == Vector2.right)
                        {
                            array[i].momentoDeColisao.x = 1;
                            if (velocity.x > 0) velocity.x = 0;
                        }
                        if (array[i].Position.y + velocity.y > array[j].Position.y && dir == Vector2.up)
                        {
                            array[i].momentoDeColisao.w = 1;
                            if (velocity.y < 0) velocity.y = 0;
                        }
                        if (array[i].Position.y + velocity.y < array[j].Position.y && dir == Vector2.up)
                        {
                            array[i].momentoDeColisao.y = 1;
                            if (velocity.y > 0) velocity.y = 0;
                        }

                    }

                    array[i].onColisionAction?.Invoke(array[j]);

                }
            }
        }
    }
}
