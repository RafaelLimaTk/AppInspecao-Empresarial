﻿using InspecaoEmpresarial.Domain;

namespace Inspeção_Empresarial.Repositories;

public interface IEstablishmentRepository
{
    IList<Establishment> GetAll();
    Establishment GetById(int id);
    void Add(Establishment establishment);
    void Update(Establishment establishment);
    void Delete(Establishment establishment);
}

