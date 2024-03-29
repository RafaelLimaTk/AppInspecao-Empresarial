﻿using InspecaoEmpresarial.Domain;

namespace Inspeção_Empresarial.Repositories;

public interface IResponsibilityRepository
{
    IList<Responsibility> GetAll();
    Responsibility GetById(int id);
    void Add(Responsibility responsibility);
    void Update(Responsibility responsibility);
    void Delete(Responsibility responsibility);
}
