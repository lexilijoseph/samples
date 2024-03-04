using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HierarchyData.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        //private List<FolderHierarchy>  searchAll(List<FolderHierarchy> lst, string parent, string child)
        //{
        //    if (lst.Count() > 0)
        //    {
        //        for (int a = 0; a <= lst.Count(); a++)
        //        {
        //            if (parent == lst[a].ParentFolder)
        //            {
        //                //List<FolderHierarchy> childList = (from item in lst[a]
        //                //                                   where item.ParentFolder == parent
        //                //                                   select item).ToList();
        //                //FolderHierarchy obj = new FolderHierarchy();
        //                //obj.ParentFolder = nodeList[i];
        //                //// lstChildFolders.Add(obj);
        //                //childList[0].ChildFolders.Add(obj);
        //            }
        //            searchAll(lst[a].ChildFolders, parent, child);
        //        }
        //    }
        //}
        private List<FolderHierarchy> AddParent(List<string> root)
        {
            List<FolderHierarchy> lst = new List<FolderHierarchy>();
            return lst;
        }
        private List<FolderHierarchy> AddFolderHierarchy(List<FirstResultModel> firstResultModel)
        {
            // fetch parent
            List<FolderHierarchy> lst = new List<FolderHierarchy>();
           var parent = (from item in firstResultModel select new { item.FirstNode }).ToList().Distinct();
            foreach (var node in parent)
            {              
                FolderHierarchy obj = new FolderHierarchy();
                obj.ParentFolder = node.FirstNode;
                lst.Add(obj);
            }
         //  var nodeList = node.FoldersList;
              foreach (var node in firstResultModel)
              {
                var nodeList = node.FoldersList;
                  int i = 1;
                var currentNode = nodeList[node.Level];
                var ParentNode = from item in lst select item.ChildFolders;
                while (i<=node.Level)
                  {
                      var s = nodeList[i];
                  //  if(node)
                    //  searchAll(lst, nodeList[i - 1], nodeList[i]);
                      //parent exist
                      List<FolderHierarchy> current = lst.Skip(i - 1).ToList();
                      var exist = (from item in current
                                   where item.ParentFolder == nodeList[i-1]
                                   select item).Count(); //lst.Count(a => a.ParentFolder == nodeList[i - 1]);


                      if (exist == 0)
                      {
                          FolderHierarchy obj = new FolderHierarchy();
                          obj.ParentFolder = nodeList[i - 1];
                          List<FolderHierarchy> ChildFolders = new List<FolderHierarchy>();
                          FolderHierarchy child = new FolderHierarchy();
                          child.ParentFolder = nodeList[i];
                          ChildFolders.Add(child);
                          obj.ChildFolders = ChildFolders;
                          lst.Add(obj);
                      }
                      else
                      {
                          List<FolderHierarchy> childList = (from item in lst
                                                             where item.ParentFolder == nodeList[i-1]
                                                             select item).ToList();
                          FolderHierarchy obj = new FolderHierarchy();
                          obj.ParentFolder = nodeList[i];
                          // lstChildFolders.Add(obj);
                        //  childList[i].ChildFolders.Add(obj);
                      }
                      i++;
                  }
                 //if (node.Level==0)
                 // {                   

                 //     FolderHierarchy obj = new FolderHierarchy();
                 //     obj.ParentFolder = node.FirstNode;
                 //     lst.Add(obj);
                 // }
              }
            return lst;
        }

        [HttpGet(Name = "GetFolderDetails")]
        public List<FolderHierarchy> Get()
        {
            List<FolderModel> lstOrg = new List<FolderModel>();


            lstOrg.Add(new FolderModel { FolderId = 2, Fullpath = "Parent1|Child1", IsFolder = true });
            lstOrg.Add(new FolderModel { FolderId = 4, Fullpath = "Parent3", IsFolder = true });
            lstOrg.Add(new FolderModel { FolderId = 6, Fullpath = "Parent1|Child2|1.txt", IsFolder = false });
            lstOrg.Add(new FolderModel { FolderId = 7, Fullpath = "Parent1|Child2|2.txt", IsFolder = false });
            lstOrg.Add(new FolderModel { FolderId = 8, Fullpath = "Parent1|Child2|GrandChild1|3.txt", IsFolder = false });
            lstOrg.Add(new FolderModel { FolderId = 9, Fullpath = "Parent2|1.txt", IsFolder = false });

            // lstOrg.Add(new FolderModel { FolderId = 1, Name = "Parent1", Fullpath = null, IsFolder = true });
            //lstOrg.Add(new FolderModel { FolderId = 2, Name = "Child1", Fullpath = "Parent1|Child1", IsFolder = true });
            //lstOrg.Add(new FolderModel { FolderId = 3, Name = "Parent2", Fullpath = null, IsFolder = true });
            //lstOrg.Add(new FolderModel { FolderId = 4, Name = "Parent3", Fullpath = "Parent3", IsFolder = true })
            //lstOrg.Add(new FolderModel { FolderId = 5, Name = "Child2", Fullpath = "Parent1|Child2", IsFolder = true });
            //lstOrg.Add(new FolderModel { FolderId = 6, Name = "1.txt", Fullpath = "Parent1|Child2", IsFolder = false });
            //lstOrg.Add(new FolderModel { FolderId = 7, Name = "2.txt", Fullpath = "Parent1|Child2", IsFolder = false });
            //lstOrg.Add(new FolderModel { FolderId = 8, Name = "3.txt",Fullpath= "Parent1|Child2|GrandChild1", IsFolder = false });
            //lstOrg.Add(new FolderModel { FolderId = 9, Name = "1.txt",Fullpath= "Parent2", IsFolder = false });

            var parentList = lstOrg.Where(i => i.Fullpath.Contains("|"));
            //var parentList = from p in lstOrg
            //                 where p.IsFolder==true && p.Fullpath==null
            //              select p;

            List<FolderHierarchy> lsth = new List<FolderHierarchy>();
            var flataData = (from p in lstOrg
                          select new FirstResultModel
                          {
                              Level = p.Fullpath.Count(x => x == '|'),
                              FirstNode = p.Fullpath.Split("|").First(),
                              LastNode = p.Fullpath.Split("|").Last(),
                              FoldersList = p.Fullpath.Split('|').ToList()

                          }).ToList();
            var k = AddFolderHierarchy(flataData);//.GroupBy(x => x.ParentFolder) ;
                  var result1 = from p in lstOrg
                          select new
                          {
                              p.FolderId,
                              //folderlist = (p.Fullpath.Split("|").Aggregate((s1, s2) => s1 + ", " + s2)),
                              lst = p.Fullpath,
                              Parent=p.Fullpath.Split("|").First(),
                              last=p.Fullpath.Split("|").Last(),
                              level = p.Fullpath.Count(x => x == '|'),
                              FolderLis =(p.Fullpath.Split('|').Aggregate(new List<string>(), (list, word) => { var c = (list.Any() ? $"{list.Last()}-{word}" : word); list.Add(c); return list; })),

                              FolderList1 = (p.Fullpath.Split('|').Aggregate(new List<string>(), (list, word) => {
                                  //if (list.Any())
                                  //{
                                  //    FolderModel objParent = new FolderModel { FolderId = p.FolderId,Fullpath= word };
                                  //    List<FolderModel> lstChildFolders = new List<FolderModel>();
                                  //    FolderModel objchild = new FolderModel { FolderId = p.FolderId, Fullpath = word };
                                  //    lsth.Add(new FolderHierarchy { ParentFolder = objParent, ChildFolders = lstChildFolders });
                                  //}
                                  var c = (list.Any() ? $"{list.Last()}-{word}" : word); 
                                  if(!c.Contains('-'))
                                  {
                                      string[] s = c.Split('-');
                                      
                                      // FolderHierarchy objParent = new FolderHierarchy { ParentFolder = word };
                                      // List<FolderModel> lstChildFolders = new List<FolderModel>();
                                      var parentexist = lsth.Count(a => a.ParentFolder == s[0]);
                                      if (parentexist == 0)
                                      {
                                          List<FolderHierarchy> lstChildFolders = new List<FolderHierarchy>();
                                          //FolderModel objchild = new FolderModel { FolderId = p.FolderId, Fullpath = word };
                                          lsth.Add(new FolderHierarchy { ParentFolder = word, ChildFolders = lstChildFolders });
                                      }
                                      else
                                      {
                                    
                                      }
                                  }
                                  else
                                  {
                                       
                                      // var k=list.Find()
                                      // FolderModel objParent = new FolderModel { FolderId = p.FolderId, Fullpath = word };
                                      var parentexist = lsth.Count(a => a.ParentFolder == list.Last());
                                      if (parentexist == 0)
                                      {
                                          List<FolderHierarchy> lstChildFolders = new List<FolderHierarchy>();
                                          FolderHierarchy obj = new FolderHierarchy();
                                          obj.ParentFolder = word;
                                          lstChildFolders.Add(obj);
                                          //FolderModel objchild = new FolderModel { FolderId = p.FolderId, Fullpath = word };
                                          lsth.Add(new FolderHierarchy { ParentFolder = list.Last(), ChildFolders = lstChildFolders });
                                      }
                                      else
                                      {
                                          var exist = lsth.Count(a => a.ParentFolder == list.Last() && a.ParentFolder == word);
                                          if (exist == 0)
                                          {
                                              List<FolderHierarchy> childList = (from item in lsth
                                                                                 where item.ParentFolder == list.Last()
                                                                                 select item).ToList();
                                              FolderHierarchy obj = new FolderHierarchy();
                                              obj.ParentFolder = word;
                                              // lstChildFolders.Add(obj);
                                              childList[0].ChildFolders.Add(obj);
                                          }
                                      }

                                  }
                                  list.Add(c);
                                  
                                  return list; 
                              }))
                              //                     dict = (p.Fullpath.Split('|')             
                              //  .ToDictionary(a => p.Fullpath.Split('|')[0].Trim(), a => p.Fullpath.Split('|')[1].Trim())),
                              //list = p.Fullpath.Select(m => new { Key = p.Fullpath.Split('|')[0], Value = p.Fullpath.Split('|')[1] })
                              //.GroupBy(m => m.Key)
                              //.ToDictionary(m => m.Key, m => string.Join(",", m.Select(p => p.Value)))
                          };

            foreach(var item in result1)
            {
               //var k= result1.GroupBy(x=>x.FolderLis==item.Parent)
                if (item.level==0)
                {
                    //FolderModel objParent = new FolderModel { FolderId = item.FolderId, Fullpath = item.lst[0] };
                    //List<FolderModel> lstChildFolders = new List<FolderModel>();
                    //lsth.Add(new FolderHierarchy { ParentFolder = objParent, ChildFolders = lstChildFolders });
                }
                else
                {
                    foreach(var item1 in item.lst)
                    {
                        var lastItem = item.lst.Last();
                        //if (lastItem != item1)
                        //{
                        //    FolderModel objParent = new FolderModel { FolderId = item.FolderId, Fullpath = item1 };
                        //    List<FolderModel> lstChildFolders = new List<FolderModel>();
                        //    FolderModel child = new FolderModel { FolderId = item.FolderId, Fullpath = item1 };
                        //    lstChildFolders.Add(child);
                        //    lsth.Add(new FolderHierarchy { ParentFolder = objParent, ChildFolders = lstChildFolders });
                        //}
                        //else
                        //{
                        //    FolderModel objParent = new FolderModel { FolderId = item.FolderId, Fullpath = item1 };
                        //    List<FolderModel> lstChildFolders = new List<FolderModel>();
                        //    lsth.Add(new FolderHierarchy { ParentFolder = objParent, ChildFolders = lstChildFolders });
                        //}
                    }

                }
            }
            var s = lsth.GroupBy(x=>x.ParentFolder);
            var dotSeperatedGroups = (from p in lstOrg select p).ToLookup(s => s.Fullpath.Contains('|'));
            var result = from p in lstOrg
                         select p.Fullpath.Split("|")
                .Aggregate(
    new { Sum = 0, List = new List<FolderModel> () },
    (data, value) =>
    {
        var d = data.List;
        int sum = data.Sum + 1;
        //if(data.any)
        //if (data.List.Count > 0 && sum <= maxSum)
        //    data.List[data.List.Count - 1].Add(value);
        //else
        //    data.List.Add(new FolderModel { (sum = value) });
       return new { Sum = sum, List = data.List };
    },
    data => data.List)
    .ToList();
  foreach(var item in result)
            {


            }
            return lsth;
        }
    }
}
