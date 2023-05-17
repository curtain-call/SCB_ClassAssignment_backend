using System.Collections.Generic;

public interface IJobPositionService
{
    // 获取所有职位
    List<JobPosition> GetAll();

    // 根据ID获取职位
    JobPosition GetById(int id);

    // 创建新的职位
    void Create(JobPosition jobPosition);

    // 更新已存在的职位
    void Update(JobPosition jobPosition);

    // 删除职位
    void Delete(int id);
}
