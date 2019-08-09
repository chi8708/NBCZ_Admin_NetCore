<template>
  <div>
    <Tree :data="DeptTree" @on-select-change="deptTreeChange"></Tree>
  </div>
</template>
<script>
import { getDepts } from "@/api/pubDept";
export default {
  props:{
    parent:{}
  },
  data() {
    return {
      Depts: [],
      DeptTree: [],
      DeptTreeItems: []
    };
  },
  methods: {
    getDepts() {
      this.Depts=[];
      this.DeptTree=[];
      this.DeptTreeItems=[];
      getDepts()
        .then(res => {
          const resData = res.data;
          const data = resData.data;
          this.Depts = data;
          this.loadDeptTree();
        })
        .catch(err => {});
    },
    loadDeptTree() {
      var treeRoot = this.Depts.filter(p => p.parentCode == "");
      this.loadDeptTreeChild(treeRoot);
    },
    loadDeptTreeChild(treeData) {
      for (let index = 0; index < treeData.length; index++) {
        const element = treeData[index];
        this.loadDeptTreeItem(element);
        const child = this.Depts.filter(p => p.parentCode == element.deptCode);
        if (child && child.length > 0) {
          this.loadDeptTreeChild(child);
        }
      }
    },
    loadDeptTreeItem(treeItemData) {
      var treeItem = {
        title: treeItemData.deptName,
        expand: true,
        value: treeItemData.deptCode
      };
      this.DeptTreeItems.push(treeItem);
      if (treeItemData.parentCode) {
        //foreach 无法终止循环
        this.DeptTreeItems.every(element => {
          if (element.value == treeItemData.parentCode) {
            if (!element.children) {
              element.children = [];
            }
            element.children.push(treeItem);
            return false;
          }
          return true;
        });
      } else {
        this.DeptTree.push(treeItem);
      }
    },
    deptSelect() {
      if (!this.Depts || this.Depts.length <= 0) {
        this.getDepts();
      }
    },
    deptTreeChange(data) {
      var item0 = data[0];
      this.parent.deptChange(item0.value,item0.title);
    }
  },
  mounted() {
    this.getDepts();
  }
};
</script>