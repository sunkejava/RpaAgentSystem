<script setup lang="ts">
import {ref,computed} from 'vue'
const tab=ref('dashboard');const company=ref('');const operation=ref('SI_EMPLOYEE_ADD');const source=ref('Excel');const result=ref('');
const pages=[['dashboard','运营总览'],['companies','企业档案'],['systems','政务系统'],['accounts','系统账号'],['business','业务办理'],['auth','认证协作'],['executions','任务监控'],['workflows','工作流'],['workers','Worker资源']];
const heading=computed(()=>pages.find(x=>x[0]===tab.value)?.[1]??'RpaAgentSystem')
function preview(){result.value='预检查仅在本地演示。实际路由需由后端验证企业授权、系统账号、工作流与执行节点。'}
</script>
<template>
<el-container style="min-height:100vh">
<el-aside width="220px" style="background:#17243a;color:#fff;padding:20px">
<h2>RpaAgentSystem</h2><el-menu :default-active="tab" background-color="#17243a" text-color="#c9d5e6" active-text-color="#fff" @select="v=>tab=v">
<el-menu-item v-for="p in pages" :key="p[0]" :index="p[0]">{{p[1]}}</el-menu-item>
</el-menu></el-aside>
<el-main style="background:#f5f7fb;padding:30px"><h2>{{heading}}</h2><el-alert title="开发原型：未接入真实政务系统，不包含生产数据" type="info" :closable="false"/>
<el-card v-if="tab==='business'" style="margin-top:20px;max-width:780px">
<el-form label-width="110px">
<el-form-item label="企业名称"><el-input v-model="company" placeholder="请选择或输入企业"/></el-form-item>
<el-form-item label="业务类型"><el-select v-model="operation"><el-option label="社保增员" value="SI_EMPLOYEE_ADD"/><el-option label="社保减员" value="SI_EMPLOYEE_REMOVE"/><el-option label="医保缴费查询" value="MI_PAYMENT_QUERY"/></el-select></el-form-item>
<el-form-item label="数据来源"><el-select v-model="source"><el-option label="Excel" value="Excel"/><el-option label="ERP API" value="API"/><el-option label="手工录入" value="Manual"/></el-select></el-form-item>
<el-form-item><el-button type="primary" @click="preview">模拟业务路由</el-button></el-form-item>
</el-form><el-alert v-if="result" :title="result" type="warning" :closable="false"/></el-card>
<el-card v-else style="margin-top:20px"><el-empty :description="heading+'：模块骨架，待后端 API 完成后联调'"/></el-card>
</el-main></el-container>
</template>