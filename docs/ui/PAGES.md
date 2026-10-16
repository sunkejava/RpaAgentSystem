# 前端页面原型规格 v1
Vue 3 + TypeScript + Vite + Pinia + Element Plus + Vue Flow。
导航：
/dashboard 总览；/companies 企业档案；/systems 政务系统；/accounts 企业账号；/business/new 业务办理；/business/batches 批量任务；/auth/challenges 认证协作；/executions 执行中心；/workflows 工作流；/workers 执行资源；/settings 设置。
业务主流程：选择企业→选择业务→选择数据源→预校验→路由预演→显示系统/账号/工作流/Worker→提交→人工认证→执行→结果与回执。
交互要求：分页搜索、权限校验、空状态、失败反馈、操作确认、敏感字段脱敏；Unknown 状态不得一键重试提交。
原型为静态模拟数据，不执行真实业务。